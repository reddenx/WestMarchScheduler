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
if [ -f "/etc/nginx/sites-available/$nginxConfig" ];
then
    echo "site is already configured"
    exit 2;
fi


# copy over current configuration
echo "step 1/3 deploying configuration and setting up links"
sudo cp ./$nginxConfig /etc/nginx/sites-available/$nginxConfig
sudo ln -s /etc/nginx/sites-available/$nginxConfig /etc/nginx/sites-enabled/$nginxConfig

# test the configuration
echo "step 2/3 testing configuration"
sudo nginx -t

# reset nginx
echo "step 3/3 loading configuration"
sudo nginx -s reload

echo "deployment complete"
exit 0;