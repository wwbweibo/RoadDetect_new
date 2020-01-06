From python
ADD . /work
WORKDIR /work
RUN pip install -r requirements.txt
RUN pip install torch==1.3.1+cpu torchvision==0.4.2+cpu -f https://download.pytorch.org/whl/torch_stable.html
CMD "python" "runserver.py"