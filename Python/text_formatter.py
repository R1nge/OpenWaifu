import re

class Text_Formater:
    def clean_emotion_action_text_for_speech(self, text:str):
        clean_text = re.sub(r'\*.*?\*', '', text) # remove *action* from text
        clean_text = clean_text.replace(f'You: ', '') # replace -> name: "dialog"
        return clean_text