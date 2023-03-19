nginxConfig=schedule.daystone.club
nginxConfigBackup=backup_schedule.daystone.club


# ensure running with appropriate privilages
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# does the configuration exist to deploy?
if [ ! -f "./$nginxConfig" ];
then
    echo "nginx config not found, did not publish";
    exit 2;
fi

# is the site already set up?
if [ ! -f "/etc/nginx/sites-available/$nginxConfig" ];
then
    echo "site is not already configured"
    exit 2;
fi

# copy the current site to a backup
echo "step 1/4 backing up"
sudo cp /etc/nginx/sites-available/$nginxConfig ./$nginxConfigBackup

# copy over current configuration
echo "step 2/4 deploying configuration"
sudo cp ./$nginxConfig /etc/nginx/sites-available/$nginxConfig

# test the configuration
echo "step 3/4 testing configuration"
sudo nginx -t

# reset nginx
echo "step 4/4 loading configuration"
sudo nginx -s reload

echo "deployment complete"
exit 0;