import React from 'react';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const HomePage = () => {
    const navigate = useNavigate();

    return (
        <div  style={{ padding: '30px',  boxSizing: 'border-box', minHeight: '100vh' }}>
            <h1>Welcome to the Employee Cafe App</h1>
            <Button variant="contained" color="primary" onClick={() => navigate('/cafes')}>
                View Cafes
            </Button>
            <Button variant="contained" color="secondary" onClick={() => navigate('/employees')} style={{ marginLeft: '10px' }}>
                View Employees
            </Button>
        </div>
    );
};

export default HomePage;