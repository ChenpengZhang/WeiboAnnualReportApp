from snownlp import SnowNLP

def say_hello():
? ? return "Hello World!"

def process_sentence(text: str):
? ? snow = SnowNLP(text)
? ? sentiment = snow.sentiments
? ? return sentiment