from django.urls import path
from .import views
from .import views
from django.views.decorators.csrf import csrf_exempt

urlpatterns = [
    path('', views.index, name='index'),
    path('category', views.category, name="category"),
    path('input-data', views.input, name="input"),
    path('categoryDetail/<pk>', views.categoryDetail, name="categoryDetail"),
    path('buildingDetail/<pk>', views.buildingDetail, name="buildingDetail"),
    path('categoryList/<pk>', views.categoryList, name="categoryList"),
    path('flatting/<pk>', views.flatting, name="flatting"),
    path('flattingResult', views.flattingResult, name="flattingResult"),
    path('area', views.area, name="area"),
    path('createCrack/<pk>', views.createCrack,name='createCrack'),
    path('crackDetail/<pk>',views.crackDetail, name='crackDetail'),
    path('crackCrackObj/<pk>',views.createCrackObj, name='createCrackObj'),
    path('save/<pk>', csrf_exempt(views.save), name="save"),
    path('error', views.error, name="error"),
    path('createExcel/<pk>',views.createExcel,name="createExcel"),
    path('deleteCrackObj/<pk>',views.deleteCrackObj, name="deleteCrackObj"),
    path('deleteCrack/<pk>',views.deleteCrack, name="deleteCrack"),
    path('updateCrack/<pk>',views.updateCrack, name="updateCrack"),
    path('handleUpdateCrack/<pk>',views.handleUpdateCrack, name="handleUpdateCrack"),
    path('deleteCategory/<pk>',views.deleteCategory, name="deleteCategory"),
    path('updateCategory/<pk>',views.updateCategory, name="updateCategory"),
    path('handleUpdateCategory/<pk>',views.handleUpdateCategory, name="handleUpdateCategory"),
    path('searchResult/', views.searchResult, name="searchResult")

]
