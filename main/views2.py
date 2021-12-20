from django.shortcuts import render


def category(request):
  return render(request, 'category.html')

def createCategory(request):
  return render(request, 'newDataForm.html')