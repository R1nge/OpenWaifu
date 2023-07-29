from pysentimiento import create_analyzer
import re

class Emotion_Analyzer:
    def __init__(self):
        self.emotion_analyzer = create_analyzer(task="emotion", lang="en")

    def analyze(self, text:str) -> str:
        emotions_text = text
        if '*' in text:
            emotions_text = re.findall(r'\*(.*?)\*', emotions_text) # get emotion *action* as input if exist
            emotions_text = ' '.join(emotions_text) # create input
    
        emotions = self.emotion_analyzer.predict(emotions_text).probas
        ordered = dict(sorted(emotions.items(), key=lambda x: x[1]))
        ordered = [k for k, v in ordered.items()] # top two emotion
        ordered.reverse()
        return ordered[:2]