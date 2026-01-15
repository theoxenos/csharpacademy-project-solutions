export const ACTION_TYPE = {
    DELETE: 'delete',
    UPDATE: 'update',
    COMPLETE: 'complete',
    CREATE: 'create'
} as const;

export const TOAST_TYPE = {
    SUCCESS: 'success',
    ERROR: 'danger'
} as const;

export type ActionType = typeof ACTION_TYPE[keyof typeof ACTION_TYPE];
export type ToastType = typeof TOAST_TYPE[keyof typeof TOAST_TYPE];