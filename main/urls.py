from django.urls import path
from .import views
from .import views2

urlpatterns = [
    path('', views.index, name='index'),
    # ---------
    path('category', views2.category, name="category"),
    path('createCategory', views2.createCategory, name="createCategory"),
    path('categoryDetail', views2.categoryDetail, name="categoryDetail"),
    path('fileUpload', views2.fileUpload, name="fileUplad"),
]
