import React from 'react';
import { withStyles, Theme, createStyles } from '@material-ui/core/styles';
import Switch, { SwitchClassKey, SwitchProps } from '@material-ui/core/Switch';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import { ISwitchComponent } from '.';

interface Styles extends Partial<Record<SwitchClassKey, string>> {
    focusVisible?: string;
  }
  
  interface Props extends SwitchProps {
    classes: Styles;
  }

const IOSSwitch = withStyles((theme: Theme) =>
  createStyles({
    root: {
      width: 42,
      height: 26,
      padding: 0,
      margin: theme.spacing(1),
    },
    switchBase: {
      padding: 1,
      '&$checked': {
        transform: 'translateX(16px)',
        color: theme.palette.common.white,
        '& + $track': {
          backgroundColor: '#2e2e42',
          opacity: 1,
          border: 'none',
        },
      },
      '&$focusVisible $thumb': {
        color: '#2e2e42',
        border: '6px solid #2e2e42',
      },
    },
    thumb: {
      width: 24,
      height: 24,
      backgroundColor: '#fff',
    },
    track: {
      borderRadius: 26 / 2,
      border: `1px solid #e5e5e5`,
      backgroundColor: '#e5e5e5',
      opacity: 1,
      transition: theme.transitions.create(['background-color', 'border']),
    },
    checked: {},
    focusVisible: {},
  }),
)(({ classes, ...props }: Props) => {
  return (
    <Switch
      focusVisibleClassName={classes.focusVisible}
      disableRipple
      classes={{
        root: classes.root,
        switchBase: classes.switchBase,
        thumb: classes.thumb,
        track: classes.track,
        checked: classes.checked,
      }}
      {...props}
    />
  );
});

export interface IRegularSwitchComponent extends ISwitchComponent {}

const RegularSwitchComponent = (props: IRegularSwitchComponent) => {

    const [checked, setChecked] = React.useState(props.defaultValue || false);

    return (
        <FormControlLabel
            control={
                <IOSSwitch 
                    checked={checked} 
                    onChange={() => {
                        const newValue = !checked;
                        setChecked(newValue);
                        props.switchHandler && props.switchHandler(newValue);
                    }} 
                    name="checkedB"
                />}
            label="Auto-refresh"
            labelPlacement="start"
            style={{ marginRight: 5 }}
        />
    )
}

export default RegularSwitchComponent;