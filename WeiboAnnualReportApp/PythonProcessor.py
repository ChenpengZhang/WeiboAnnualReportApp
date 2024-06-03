from snownlp import SnowNLP

def say_hello():
	return "Hello World!"

def process_sentiment(text: str):
	snow = SnowNLP(text)
	sentiment = snow.sentiments
	return sentiment

def process_words(text: str):
	snow = SnowNLP(text)
	return snow.words