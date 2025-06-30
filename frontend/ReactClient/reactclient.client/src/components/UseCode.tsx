import React, { useState } from 'react';
import { Button, TextField, Typography, Paper } from '@mui/material';

export default function UseCode() {
  const [codeList, setCodeList] = useState<string[]>([]);
  const [useCode, setUseCode] = useState('');
  const [useMessage, setUseMessage] = useState('');

  // For demonstration, preload some codes or you can add your own generate logic
  React.useEffect(() => {
    setCodeList(['ABC123', 'XYZ789', 'HELLO1']); // Example codes
  }, []);

  const handleUse = () => {
    if (!codeList.includes(useCode)) {
      setUseMessage('Not found');
    } else {
      setUseMessage('Successfully used');
      // Optionally remove used code or mark it used here
    }
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
      />
      <Button variant="contained" fullWidth onClick={handleUse} sx={{ mt: 2 }}>
        Use
      </Button>
      {useMessage && (
        <Typography
          variant="body1"
          sx={{
            mt: 2,
            color:
              useMessage === 'Successfully used'
                ? 'green'
                : useMessage === 'Not found'
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
