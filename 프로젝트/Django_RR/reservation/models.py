from django.db import models

# Create your models here.

class reservation(models.Model):
    title = models.CharField(max_length=200)
    pub_date = models.DateTimeField('date published')
    body = models.TextField()

    def __str__(self):
        return self.title


class RImg_board(models.Model):
    title = models.CharField('ID', max_length=200)
    image = models.ImageField(upload_to='images/')
    description = models.TextField('Reservation Menu ')
    now_date = models.DateTimeField(auto_now=True)
    pdate = models.DateTimeField('Reservation Time ')

    def __str__(self):
        return self.title