from django import forms
from .models import reservation, RImg_board

class reservation(forms.ModelForm):
    class Meta:
        model = reservation
        fields = ['title','body']   


class RImg_Board(forms.ModelForm):
    class Meta:
        model = RImg_board
        fields=['title','image','description']