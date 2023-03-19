dateprefix=`date +%Y-%m-%d_`;

for filename in `ls -x *.sql`; do
    sudo mysql < $filename;
    mv ./${filename} ./executed/${dateprefix}-${filename}
done;