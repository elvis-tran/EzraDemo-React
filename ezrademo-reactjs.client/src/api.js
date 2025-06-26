import axios from "axios";
const API_URL = 'http://localhost:5022/api';
const tasksEndpoint = `${API_URL}/tasks`;

export async function fetchTasks() {
    try {
        const response = await axios.get(tasksEndpoint)
        if (Array.isArray(response.data)) {
            //Normalize the return data to address the camel case and pascal case issue
            return response.data.map(task => ({
                id: task.id ?? task.Id,
                taskName: task.taskName ?? task.TaskName,
                isCompleted: task.isCompleted ?? task.IsCompleted,
                taskType: task.taskType ?? task.TaskType
            }));
        }
        return [];
    }
    catch (error) {
        console.log('Failed to retrieve tasks (API):', error.response?.data || error.message);
        return [];
    }
}

export async function createTask(task) {
    try {
        const response = await axios.post(tasksEndpoint, {
            taskName: task.taskName
        });
        const taskData = response.data;

        return {
            id: taskData.id ?? taskData.Id,
            taskName: taskData.taskName ?? taskData.TaskName,
            isCompleted: taskData.isCompleted ?? taskData.IsCompleted,
            taskType: taskData.taskType ?? taskData.TaskType
        };
    }
    catch (error) {
        console.log('Create task error (API):', error.response?.data || error.message);
    }
}

export async function toggleTask(id) {
    try {
        const response = await axios.patch(`${API_URL}/tasks/${id}`);
        const taskData = response.data;

        return {
            id: taskData.id ?? taskData.Id,
            taskName: taskData.taskName ?? taskData.TaskName,
            isCompleted: taskData.isCompleted ?? taskData.IsCompleted,
            taskType: taskData.taskType ?? taskData.TaskType
        };
    } catch (error) {
        console.error('Toggle task failed (API):', error.response?.data || error.message);
        throw error;
    }
}

export async function deleteCompletedTasks() {
    try {
        const response = await axios.delete(`${API_URL}/tasks/completed`);
        return response.data;  // Number of deleted tasks
    } catch (error) {
        console.error('Failed to delete completed tasks (API):', error.response?.data || error.message);
        throw error;
    }
}
