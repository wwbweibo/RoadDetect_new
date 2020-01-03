"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template
from CrackPreProcess import app
from CrackPreProcess.Kafka  import Client
from CrackPreProcess.Utils import Decodeb64String, DecodeByte2Image
from CrackPreProcess.Service.PreProcessService import PreProcessService

@app.route('/')
@app.route('/home')
def home():
   
    """Renders the home page."""
    return render_template(
        'index.html',
        title='Home Page',
        year=datetime.now().year,
    )

@app.route('/contact')
def contact():
    """Renders the contact page."""
    return render_template(
        'contact.html',
        title='Contact',
        year=datetime.now().year,
        message='Your contact page.'
    )

@app.route('/about')
def about():
    """Renders the about page."""
    return render_template(
        'about.html',
        title='About',
        year=datetime.now().year,
        message='Your application description page.'
    )

@app.route("/perprocess", methods=['POST'])
def PreProcess(imageb64data):
    # basic validation
    if imageb64data is None or len(imageb64data) <=0:
        raise("the input data can not be None or empty")
    service = PreProcessService(imageb64data)
