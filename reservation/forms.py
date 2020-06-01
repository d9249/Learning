from django import forms
from .models import reservation, RImg_board

class reservation(forms.ModelForm):
    class Meta:
        model = reservation
        fields = ['title','pub_date','body']   


class RImg_Board(forms.ModelForm):
    class Meta:
        model = RImg_board
        fields=['title','description','pdate']