from django.urls import path
from .import views
from .import views2

urlpatterns = [
    path('', views.index, name='index'),
    # ---------
    path('category', views2.category, name="category")
]
