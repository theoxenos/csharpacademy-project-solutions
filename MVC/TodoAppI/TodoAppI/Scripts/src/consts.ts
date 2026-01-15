export const ACTION_TYPE = {
    DELETE: 'delete',
    UPDATE: 'update',
    COMPLETE: 'complete',
    CREATE: 'create'
} as const;

export type ActionType = typeof ACTION_TYPE[keyof typeof ACTION_TYPE];