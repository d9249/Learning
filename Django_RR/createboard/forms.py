from django import forms
from .models import Board, Img_board

class CreateBoard(forms.ModelForm):
    class Meta:
        model = Board
        fields = ['title','body']   


class Img_Board(forms.ModelForm):
    class Meta:
        model = Img_board
        fields=['title','image','description']