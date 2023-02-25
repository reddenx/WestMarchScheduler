# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# check if things are where we think they are
if [ ! -f "./schedule.daystone.club" ]; 
then
    echo "deployment not found, abandoning publish";
    exit 2;
fi

if [ ! -f "/etc/nginx/sites-available/schedule.daystone.club" ]; 
then
    echo "destination not found, abandoning publish";
    exit 2;
fi



# copy rss.daystone.club to nginx_backup
echo "step 1/4 creating backup"
sudo cp /etc/nginx/sites-available/schedule.daystone.club ./.backup-schedule.daystone.club

# copy new to rss.daystone.club
echo "step 2/4 deploying configuration"
sudo cp ./nginx.txt /etc/nginx/sites-available/schedule.daystone.club

# test nginx
echo "step 3/4 testing configuration"
sudo nginx -t

# reset nginx
echo "step 4/4 loading configuration"
sudo nginx -s reload

echo "-- deployment complete --"
exit 0;