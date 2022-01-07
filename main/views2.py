from django.shortcuts import render, redirect


def category(request):
    return render(request, 'category.html')


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
