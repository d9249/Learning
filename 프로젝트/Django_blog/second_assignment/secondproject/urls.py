
from django.contrib import admin
from django.urls import path, include
import blog.views
import portfolio.views
from django.conf import settings
from django.conf.urls.static import static


urlpatterns = [
    path('admin/', admin.site.urls),
    path('blog/', blog.views.home, name='home'),
    path('', blog.views.main, name='main'),
    path('blog/', include('blog.urls')),
    path('portfolio/', include('portfolio.urls')),
    path('accounts/', include('accounts.urls')),
]

urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
urlpatterns += static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
