import React, { useState, useEffect } from 'react';
import axios from 'axios';
import TextBox from './TextBox';
import { Button, Dialog, Box,FormLabel, DialogActions, DialogContent, DialogTitle } from '@mui/material';


const AddEditCafe = ({ onClose, cafeId, onSubmit }) => {
    const [cafe, setCafe] = useState({
        name: '',
        description: '',
        logo: null,
        location: ''
    });
    const [isDirty, setIsDirty] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (cafeId) {
            axios.get('https://localhost:7032/api/Cafe')
                .then(response => {
                    const selectedCafe = response.data.find(cafe => cafe.id === parseInt(cafeId));
                    if (selectedCafe) {
                        setCafe(selectedCafe);
                    }
                })
                .catch(error => {
                    console.error("There was an error fetching the cafes:", error);
                });
        }
    }, [cafeId]);


    const handleChange = (e) => {
        const { name, value, files, type } = e.target;
        if (type === 'file' && files[0]) {
            const file = files[0];
            if (file.size > 2 * 1024 * 1024) { // 2MB
                setError('File size should not exceed 2MB');
                return;
            } else {
                setError('');
                setCafe((prevCafe) => ({
                    ...prevCafe,
                    [name]: file
                }));
            }
        } else {
            setCafe((prevCafe) => ({
                ...prevCafe,
                [name]: value
            }));
        }
        setIsDirty(true);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (cafe.name.length < 6 || cafe.name.length > 10) {
            setError('Name must be between 6 to 10 characters length');
            return;
        }
        if (cafe.description.length > 256) {
            setError('Description must be less than 256 characters long');
            return;
        }
        if (cafe.location.length < 1) {
            setError('Location is required.');
            return;
        }
        if (cafeId) {
             updateCafe(cafeId, cafe);
        } else {
            addCafe(cafe);
        }
        onSubmit();
        onClose();
    };
    const addCafe =  (cafe) => {
        console.log('Adding new cafe with data:', cafe);
        try {
            const formData = new FormData();
            formData.append('name', cafe.name);
            formData.append('description', cafe.description);
            if (cafe.logo) {
                formData.append('logo', cafe.logo.name);
            }
            else{
                formData.append('logo', '');
            }
            formData.append('location', cafe.location);

            const response = axios.post('https://localhost:7032/api/Cafe', formData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            console.log('Cafe added successfully:', response.data);
        } catch (err) {
            console.error('Error adding cafe:', err.response ? err.response.data : err.message);
        }
    };
    const updateCafe = async (cafeId, cafe) => {
        console.log('Updating cafe with data:', cafe);
        try {
            const formData = new FormData();
            formData.append('Id', cafe.id);
            formData.append('name', cafe.name);
            formData.append('description', cafe.description);
            if (cafe.logo) {
                formData.append('logo', cafe.logo);
            }
            formData.append('location', cafe.location);

            const response = await axios.put(`https://localhost:7032/api/Cafe?id=${cafeId}`, formData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            console.log('Cafe updated successfully:', response.data);
        } catch (err) {
            console.error('Error updating cafe:', err.response ? err.response.data : err.message);
        }
    };
    const handleCancel = () => {
        if (isDirty && !window.confirm('You have unsaved changes. Are you sure you want to leave?')) {
            return;
        }
        onClose();
    };

    return (
        <Dialog open onClose={handleCancel} maxWidth="sm" fullWidth>
            <DialogTitle><h2>{cafeId ? 'Edit Cafe' : 'Add Cafe'}</h2></DialogTitle>
            <DialogContent>

                <form onSubmit={handleSubmit}>
                    <Box display="flex" alignItems="center" mb={1}>
                        <FormLabel sx={{ mr: 6 }}>Name:</FormLabel>
                        <TextBox
                            value={cafe.name}
                            onChange={handleChange}
                            name="name"
                            minLength={6}
                            maxLength={10}
                            required
                            />
                    </Box>
                    <Box display="flex" alignItems="center" mb={1}>
                        <FormLabel sx={{ mr: 1 }}>Description:</FormLabel>
                        <TextBox
                            value={cafe.description}
                            onChange={handleChange}
                            name="description"
                            maxLength={256}
                            />
                    </Box>
                    <Box display="flex" alignItems="center" mb={1}>
                        <FormLabel sx={{ mr: 3 }}>Location:</FormLabel>
                        <TextBox
                            value={cafe.location}
                            onChange={handleChange}
                            name="location"
                        />
                    </Box>
                    <div className="form-row">
                        <FormLabel sx={{ mr: 6 }}>Logo:</FormLabel>
                        <input
                            type="file"
                            name="logo"
                            accept="image/*"
                            onChange={handleChange}
                        />
                        {error && <p style={{ color: 'red' }}>{error}</p>}
                    </div>
                    
                </form>

            </DialogContent>
            <DialogActions>
                <Button onClick={handleSubmit} color="primary">Submit</Button>
                <Button onClick={handleCancel} color="secondary">Cancel</Button>
            </DialogActions>
        </Dialog>
    );
};

export default AddEditCafe;