
from django.contrib import admin
from django.urls import path, include
import base.views
import createboard.views

urlpatterns = [
    path('readboard/', createboard.views.readboard, name="readboard"),
    path('post/',createboard.views.post, name="post"),
    path('search/',createboard.views.search, name="search"),
    path('post/<int:img_board_id>/',createboard.views.detail,name="detail"),
    path('delete/<int:pk>/',createboard.views.delete, name="delete"),
    path('update/<int:pk>/',createboard.views.update, name="update"),
]
