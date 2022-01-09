from django.urls import path
from .import views
from .import views2

urlpatterns = [
    path('', views.index, name='index'),
    # ---------
    path('category', views2.category, name="category"),
    path('input-data', views2.input, name="input"),
    path('categoryDetail/<pk>', views2.categoryDetail, name="categoryDetail"),
    path('categoryList/<pk>', views2.categoryList, name="categoryList"),
    path('flatting/<pk>', views2.flatting, name="flatting"),
    path('flattingResult', views2.flattingResult, name="flattingResult"),
    path('area', views2.area, name="area"),
    path('createCrack/<pk>', views2.createCrack,name='createCrack')

]
