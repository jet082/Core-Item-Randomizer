import json, random

itemList = set()

def Parse(itemLogic, isOr=True):
		if isinstance(itemLogic, str):
			if itemLogic in gameLogic['logic']:
				Parse(gameLogic['logic'][itemLogic])
			else:
				itemList.add(itemLogic)
		else:
			if isOr:
				toParse = random.choice(itemLogic)
				Parse(toParse, not isOr)
			else:
				for someCondition in itemLogic:
					Parse(someCondition)
with open("DefaultLogic.json") as f:
	gameLogic = json.loads(f.read())
	keyToParse = random.choice(list(gameLogic['supplyCrateCoordinates'].keys()))
	Parse(gameLogic['supplyCrateCoordinates'][keyToParse]['logic'])
print(keyToParse)
print(itemList)