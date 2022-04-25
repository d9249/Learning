from django import forms
from .models import Blog

<<<<<<< HEAD
class BlogPost(forms.Form):
    email = forms.EmailField()
    files = forms.FileField()
    url = forms.URLField()
    words = forms.CharField(max_length=200)
    max_number = forms.ChoiceField(choices=[('1','one'),('2','two'),('3','three')])
=======
class BlogPost(forms.ModelForm):
    class Meta:
        model = Blog
        fields = ['title', 'body']
>>>>>>> 78688142efa2524627f3b1d5ed50a34618361461
