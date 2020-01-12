#From ubuntu
#ADD . /work
#WORKDIR /work
#RUN apt update && apt install python python3-pip -y
#RUN pip3 install -r requirements.txt
#RUN pip3 install torch==1.3.1+cpu torchvision==0.4.2+cpu -f https://download.pytorch.org/whl/torch_stable.html
#CMD "python" "runserver.py"

FROM tensorflow/tensorflow:latest-py3
ADD . /work
WORKDIR /work
RUN pip install -r requirements.txt
CMD "python" "runserver.py"