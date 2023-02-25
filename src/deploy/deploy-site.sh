# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# move to correct relative project directory
cd ..

# install components client app
echo "step 1/5 client install STARTING"
sudo -u ec2-user npm install --prefix ./scheduler-client >> publish.log
echo "step 1/5 client install COMPLETE"

# build client app
echo "step 2/5 client build STARTING"
sudo -u ec2-user npm run build --prefix ./scheduler-client >> publish.log
echo "step 2/5 client build COMPLETE"

# stop site
echo "step 3/5 service stop STARTING"
sudo systemctl stop site_scheduler.service >> publish.log
echo "step 3/5 service stop COMPLETE"

# move to api directory
cd ./WestMarchSite

# publishes everything
echo "step 4/5 server build STARTING"
sudo dotnet publish -c Release -r linux-x64 >> publish.log
echo "step 4/5 server build COMPLETE"

# start site
echo "step 5/5 service start STARTING"
sudo systemctl start site_scheduler.service >> publish.log
echo "step 5/5 service start COMPLETE"

exit 0;
