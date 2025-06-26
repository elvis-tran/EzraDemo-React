import React, { useEffect, useState } from 'react';
import './App.css';
import { fetchTasks, createTask, toggleTask, deleteCompletedTasks } from './api';
import TaskInput from './components/TaskInput';
import TaskList from './components/TaskList';
import DeleteCompletedButton from './components/DeleteCompletedButton';

function App() {
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadTasks = async () => {
            try {
                const data = await fetchTasks();
                setTasks(data);
            } catch (err) {
                console.error('Error fetching tasks:', err);
                setError('Failed to load tasks, please try refreshing the page');
            } finally {
                setLoading(false);
            }
        };

        loadTasks();
    }, []);

    const handleCreateTask = async (taskData) => {
        try {
            const newTask = await createTask(taskData);
            if (newTask != undefined) {
                setTasks((prev) => [...prev, newTask]);
            }
        } catch (err) {
            console.error('Failed to create task', err);
        }
    };

    const handleToggleTask = async (id) => {
        setTasks(prev =>
            prev.map(task => task.id === id ? { ...task, isCompleted: !task.isCompleted } : task)
        );

        try {
            await toggleTask(id);
        } catch (err) {
            // revert on error
            setTasks(prev =>
                prev.map(task => task.id === id ? { ...task, isCompleted: !task.isCompleted } : task)
            );
            setError('Failed to update task status.');
            console.error('Failed to toggle the task', err);
        }
    };

    const hasCompletedTasks = tasks.some(task => task.isCompleted);

    const handleDeleteCompleted = async () => {
        try {
            const response = await deleteCompletedTasks();
            // Remove completed tasks from state after deletion
            setTasks(prev => prev.filter(task => !task.isCompleted));
            return response;
        } catch (err) {
            console.error('Failed to delete completed tasks', err);
            throw err;
        }
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div style={{ color: 'red' }}>{error}</div>;

    return (
        <div style={{ padding: '2rem' }}>
            <h1>My Task List</h1>
            <TaskInput onTaskCreated={handleCreateTask} />
            {Array.isArray(tasks) && tasks.length > 0 ? (
                <TaskList tasks={tasks} onToggle={handleToggleTask} />
            ) : (
                <p>No tasks yet. Please Refresh if the Swagger UI has not launched yet.</p>
            )}
            <DeleteCompletedButton
                onDeleteCompleted={handleDeleteCompleted}
                hasCompletedTasks={hasCompletedTasks}
                resetSignal={tasks.length}  // resets the delete completed message
            />
        </div>
    );
}

export default App;