import datetime

def Error(message, exception):
    f = open('log-error-'+ datetime.time().strftime('yyyy-MM-dd')+'.log', 'w')
    f.write('[%s]-[%s]-[%s]')