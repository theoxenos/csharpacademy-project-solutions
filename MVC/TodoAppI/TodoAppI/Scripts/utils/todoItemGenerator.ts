// 1️⃣ Action verbs
const verbs = [
    "Buy",      // purchase something
    "Call",     // make a phone call
    "Clean",    // tidy up
    "Email",    // send a message
    "Fix",      // repair
    "Plan",     // schedule
    "Read",     // go through text
    "Schedule", // set a time
    "Update",   // bring up-to-date
    "Write"     // create text
];

// 2️⃣ Objects / topics
const objects = [
    "report",
    "meeting",
    "budget",
    "presentation",
    "invoice",
    "website",
    "project",
    "document",
    "task",
    "proposal"
];

// 3️⃣ Modifiers / contexts
const modifiers = [
    "today",
    "tomorrow",
    "next week",
    "urgent",
    "high-priority",
    "quick",
    "long-term",
    "for client",
    "internal",
    "review"
];

// Helper to compute the cartesian product of three arrays
function generateTodoItems(v: string[], o: string[], m: string[]) {
    const result = [];

    for (let i = 0; i < v.length; i++) {
        for (let j = 0; j < o.length; j++) {
            for (let k = 0; k < m.length; k++) {
                // Combine the three parts with spaces
                result.push(`${v[i]} ${o[j]} ${m[k]}`);
            }
        }
    }

    return result;
}

// Create the full list
const allTodos = generateTodoItems(verbs, objects, modifiers).sort(() => Math.random() - 0.5);

export default allTodos;