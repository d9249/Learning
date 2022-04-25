
from django.contrib import admin
from django.urls import path, include
import base.views
import login.views
from django.conf import settings
from django.conf.urls.static import static

urlpatterns = [
    path('admin/', admin.site.urls),
    path('', base.views.base, name="base"),
    path('login/',include('login.urls')),
    path('createboard/',include('createboard.urls')),
    path('reservation/',include('reservation.urls')),
]+ static(settings.MEDIA_URL,document_root=settings.MEDIA_ROOT)


