const API_URL = "https://YOUR_API_DOMAIN";
const API_KEY = "YOUR_API_KEY";

async function loadTasks() {
    const status = document.getElementById("statusFilter").value;
    const priority = document.getElementById("priorityFilter").value;

    let url = `${API_URL}/api/tasks?limit=20&offset=0`;

    if (status) {
        url += `&status=${status}`;
    }

    if (priority) {
        url += `&priority=${priority}`;
    }

    const response = await fetch(url, {
        headers: {
            "X-API-Key": API_KEY
        }
    });

    const tasks = await response.json();

    const container = document.getElementById("tasks");
    container.innerHTML = "";

    tasks.forEach(task => {
        const div = document.createElement("div");
        div.className = "task";

        div.innerHTML = `
            <h3>${task.title}</h3>
            <p>${task.description ?? ""}</p>
            <p>Status: ${task.status}</p>
            <p>Priority: ${task.priority}</p>

            <select onchange="changeStatus('${task.id}', this.value)">
                <option value="NEW" ${task.status === "NEW" ? "selected" : ""}>NEW</option>
                <option value="IN_PROGRESS" ${task.status === "IN_PROGRESS" ? "selected" : ""}>IN_PROGRESS</option>
                <option value="DONE" ${task.status === "DONE" ? "selected" : ""}>DONE</option>
            </select>
        `;

        container.appendChild(div);
    });
}

async function createTask() {
    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const priority = document.getElementById("priority").value;

    await fetch(`${API_URL}/api/tasks`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-API-Key": API_KEY
        },
        body: JSON.stringify({
            title,
            description,
            priority
        })
    });

    await loadTasks();
}

async function changeStatus(id, status) {
    await fetch(`${API_URL}/api/tasks/${id}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            "X-API-Key": API_KEY
        },
        body: JSON.stringify({
            status
        })
    });

    await loadTasks();
}

loadTasks();
