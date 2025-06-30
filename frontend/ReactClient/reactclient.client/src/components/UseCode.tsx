import React, { useState } from 'react';
import { Button, TextField, Typography, Paper, CircularProgress } from '@mui/material';

export default function UseCode() {
  const [useCode, setUseCode] = useState('');
  const [useMessage, setUseMessage] = useState('');
  const [loading, setLoading] = useState(false);

  const handleUse = async () => {
    setUseMessage('');
    if (!useCode.trim()) {
      setUseMessage('Please enter a code');
      return;
    }

    setLoading(true);
    try {
      const response = await fetch('https://localhost:7017/api/discountcodes/use', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ code: useCode.toUpperCase() }),
      });

      if (!response.ok) {
        setUseMessage('Server error: failed to use code');
        setLoading(false);
        return;
      }

      const data = await response.json();
      if (data.success) {
        setUseMessage('Successfully used');
        setUseCode('');
      } else {
        setUseMessage(`Failed to use code (Result code: ${data.resultCode})`);
      }
    } catch (error) {
      setUseMessage('Network error');
    }
    setLoading(false);
  };

  return (
    <Paper sx={{ padding: 3 }}>
      <Typography variant="h6" gutterBottom>
        Use Code
      </Typography>
      <TextField
        label="Code"
        fullWidth
        margin="normal"
        value={useCode}
        onChange={(e) => setUseCode(e.target.value.toUpperCase())}
        disabled={loading}
      />
      <Button
        variant="contained"
        fullWidth
        onClick={handleUse}
        sx={{ mt: 2 }}
        disabled={loading}
      >
        {loading ? <CircularProgress size={24} /> : 'Use'}
      </Button>
      {useMessage && (
        <Typography
          variant="body1"
          sx={{
            mt: 2,
            color:
              useMessage === 'Successfully used'
                ? 'green'
                : useMessage.startsWith('Failed')
                ? 'red'
                : 'orange',
          }}
        >
          {useMessage}
        </Typography>
      )}
    </Paper>
  );
}
