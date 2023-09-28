set /p "version=Enter Version (x.x): "
set registry=uzairali001
set image=clouddrop-api

docker build -t %registry%/%image%:%version% -f DockerFileApi . && docker push %registry%/%image%:%version%