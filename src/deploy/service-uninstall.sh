siteConfig=site_scheduler.service
siteConfigBackup=backup_site_scheduler.service

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

if [ ! -f "/lib/systemd/system/$siteConfig" ];
then
    echo "destination not found, abandoning publish";
    exit 2;
fi

# copy current install to backup file
echo "step 1/3 backing up current version"
sudo cp /lib/systemd/system/$siteConfig $siteConfigBackup

# disable and stop all instances
echo "step 2/3 stopping, disabling, and removing service"
sudo systemctl stop $siteConfig
sudo systemctl disable $siteConfig
sudo rm /lib/systemd/system/$siteConfig

# reload configuration file
echo "step 3/3 reload service configuration"
sudo systemctl daemon-reload
systemctl reset-failed

echo "-- uninstall complete --"