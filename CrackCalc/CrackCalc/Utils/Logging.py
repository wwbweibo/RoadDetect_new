import logging
import datetime

class LogManager:
    def __init__(self, config):
        logging.basicConfig(filename = "crackpreprocess.log", level=config['log_level'])
    def info(self, message):
        loggingTime = str(datetime.datetime.now())
        logstr = '%s\t%s' % (loggingTime, message)
        logging.info(logstr)

if __name__ == "__main__":
    lm = LogManager({'log_level':'DEBUG'})
    lm.info("test-log")
    try:
        raise Exception("asdad").with_traceback()
    except Exception as e:
        lm.debug("error", e)