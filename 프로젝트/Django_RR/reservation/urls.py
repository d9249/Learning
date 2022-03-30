from django.contrib import admin
from django.urls import path, include
import base.views
import reservation.views

urlpatterns = [
    path('readreservation/', reservation.views.readreservation, name="readreservation"),
    path('postreservation/',reservation.views.postreservation, name="postreservation"),
    path('searchreservation/',reservation.views.searchreservation, name="searchreservation"),
    path('postreservation/<int:Rimg_board_id>/',reservation.views.detailreservation,name="detailreservation"),
    path('deletereservation/<int:pk>/',reservation.views.deletereservation, name="deletereservation"),
    path('updatereservation/<int:pk>/',reservation.views.updatereservation, name="updatereservation"),
]
