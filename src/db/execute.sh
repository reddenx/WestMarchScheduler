# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

dateprefix=`date +%Y-%m-%d_`;

for filename in `ls -x *.sql`; do
    sudo mysql < $filename;
    mv ./${filename} ./executed/${dateprefix}-${filename}
done;