#From ubuntu
#RUN apt update && apt install python3 python3-pip -y
#ADD . /work
#WORKDIR /work
#RUN pip3 install -r requirements.txt
#RUN pip3 install torch==1.3.1+cpu torchvision==0.4.2+cpu -f https://download.pytorch.org/whl/torch_stable.html
#CMD "python3" "runserver.py"

FROM tensorflow/tensorflow
ADD . /work
WORKDIR /work
RUN apt update && apt install python3 python3-pip -y
RUN pip3 install -r requirements.txt
CMD "python3" "runserver.py"