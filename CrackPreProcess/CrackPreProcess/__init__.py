"""
The flask application package.
"""

from flask import Flask
from CrackPreProcess.Service.PreProcessService import PreProcessService

def load_conf():
    '''
    load conf from load file system
    '''
    conf = open('conf.ini', 'r').readlines()
    conf_dict = dict()
    for line in conf:
        conf_name, conf_value = line.split('=')
        conf_dict[conf_name.replace(' ', '')] = conf_value.replace(' ', '').replace('\n','') 
    return conf_dict

conf = load_conf()

app = Flask(__name__)
service = PreProcessService(conf)

import CrackPreProcess.views
