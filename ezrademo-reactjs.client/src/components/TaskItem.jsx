import React from 'react';

function TaskItem({ task, onToggle }) {
    const handleChange = () => {
        onToggle(task.id);
    };

    return (
        <li style={{ display: 'flex', alignItems: 'center', marginBottom: '0.5rem' }}>
            <input
                type="checkbox"
                checked={task.isCompleted}
                onChange={handleChange}
                style={{ marginRight: '0.5rem' }}
            />
            <span style={{ textDecoration: task.isCompleted ? 'line-through' : 'none'}}>
                {task.taskName}
            </span>
        </li>
    );
}

export default TaskItem;
