from django.db import models

# Create your models here.


class Photo(models.Model):
    image = models.ImageField(upload_to='images/origin/%Y/%m/%d/', null=False, blank=False)
    flatting_image = models.ImageField(upload_to='flatting/', null=True)
    isFlattened = models.BooleanField(default=False)
    originWidth = models.FloatField(null=True)
    originHeight = models.FloatField(null=True)
    crackLength = models.FloatField(null=True)
    state = models.CharField(null=True, max_length=255)
    cause = models.CharField(null=True, max_length=255)
    solution = models.CharField(null=True, max_length=255)
    category = models.ForeignKey(
        'Category', null=True, on_delete=models.SET_NULL)
    datetime = models.DateTimeField(auto_now_add=True, null=True)
    def __str__(self):
        return str(self.id)

# {{ data.pub_date|date:'Y-m-d H:i' }}
class Category(models.Model):
    name = models.CharField(null=True, unique=True, max_length=255)


