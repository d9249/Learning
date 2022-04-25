from django.db import models

# Create your models here.

class Board(models.Model):
    title = models.CharField(max_length=200)
    pub_date = models.DateTimeField('date published')
    body = models.TextField()

    def __str__(self):
        return self.title


class Img_board(models.Model):
    title = models.CharField('ID',max_length=200)
    image = models.ImageField('Image',upload_to='images/')
    description = models.TextField('Content')

    def __str__(self):
        return self.title