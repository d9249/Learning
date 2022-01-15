def flatten(data):
  result = []

  for item in data:
    if type(item) == list:
      result += flatten(item)
    else:
      result += [item]
  return result


example = [[1,2,3],[4,[5,6]],7,[8,9]]

print("원본", example)
print("변환", flatten(example))