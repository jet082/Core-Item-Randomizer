import json

f = open('DefaultLogic.json')

data = json.load(f)['Logic']
finalSet = set()

def GetItems(someArray):
	for someItem in someArray:
		if (isinstance(someItem, list)):
			GetItems(someItem)
		else:
			if someItem.replace("Start", "") not in data:
				finalSet.add(someItem)

for someLogic in data.items():
	GetItems(someLogic)
print(finalSet)