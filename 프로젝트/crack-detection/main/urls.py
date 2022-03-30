from django.urls import path
from . import views
from django.views.decorators.csrf import csrf_exempt

urlpatterns = [
    path('', views.index, name='index'),
    path('info-upload/<pk>', views.infoUpload, name="infoUpload"),
    # path('result_page/' ,views.result_page, name="result_page_"),
    path('db/', views.db, name="db"),
    path('categories/', views.categories, name="categories"),
    path('info-process/<pk>', views.infoProcess, name="infoProcess"),
    path('db/<pk>', views.dbDetail, name="dbDetail"),
    path('search', csrf_exempt(views.search), name="search"),
    path('flatting/<pk>', views.flatting, name="flatting"),
    path('lengthCalc/', views.lengthCalc, name="lengthCalc"),
    path('category-detail/<name>', views.categoryDetail, name="categoryDetail"),
    path('saveCanvas/<pk>', csrf_exempt(views.saveCanvas), name="saveCanvas"),
    path('update/<pk>', views.update, name="update"),
    path('updatePost/<pk>', views.updatePost, name="updatePost"),
    path('deletePost/<pk>', views.deletePost, name="deletePost"),
]
