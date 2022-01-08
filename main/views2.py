from django.http.response import Http404
from django.shortcuts import get_object_or_404, render, redirect
from .models import Category, Crack


def category(request):
    categorys = Category.objects.all()
    return render(request, 'category.html', {
        'categorys': categorys
    })


def input(request):
    return render(request, 'input.html')


def save(request):
    return redirect("/")


def categoryList(request):
    return render(request, 'categoryList.html')


def categoryDetail(request):
    return render(request, 'categoryDetail.html')


def flatting(request):
    return render(request, 'flatting.html')


def flattingResult(request):

    return render(request, 'flattingResult.html')


def area(request):
    return render(request, 'area.html')
