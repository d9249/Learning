from django.contrib import admin
from .models import Crack, Category, CrackObj
# Register your models here.
admin.site.register(Category)
admin.site.register(Crack)
admin.site.register(CrackObj)
