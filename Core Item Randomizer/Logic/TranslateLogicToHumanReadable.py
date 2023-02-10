import json

def parse_logic(logic_list, is_or):
	if isinstance(logic_list, str):
		return logic_list
	elif len(logic_list) == 1:
		return parse_logic(logic_list[0], not is_or)
	elif is_or:
		return f"({' or '.join(parse_logic(x, not is_or) for x in logic_list)})"
	else:
		return f"({' and '.join(parse_logic(x, not is_or) for x in logic_list)})"

# Load the logic from the JSON file
with open('DefaultLogic.json', 'r') as f:
	logic = json.load(f)

# Parse each entry in the logic
human_readable_logic = {}
for name, entry in logic.items():
	human_readable_logic[name] = {}
	for logicName, logicEntry in entry.items():
		human_readable_logic[name][logicName] = parse_logic(logicEntry, True)

# Write the human-readable logic to a file
with open('DefaultLogicHumanReadable.txt', 'w') as f:
	for name, entry in human_readable_logic.items():
		f.write(f"--{name}\n")
		for logicName, logicEntry in entry.items():
			f.write(f"{logicName}: {logicEntry}\n")
